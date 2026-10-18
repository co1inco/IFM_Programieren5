use bevy::prelude::*;
use crate::point2d::Point2D;

#[derive(Debug, Clone, Resource)]
pub struct Obj {
    pub vertices: Vec<Point2D>,
    pub faces: Vec<Vec<usize>>,
}

pub fn parse_obj(file: &str) -> Obj {
    let content = std::fs::read_to_string(file).unwrap_or_else(|e| panic!("Failed to read OBJ file '{}': {}", file, e));

    let mut obj = Obj {
        vertices: Vec::new(),
        faces: Vec::new(),
    };

    for line in content.lines() {
        let trimmed = line.trim();
        if trimmed.is_empty() || trimmed.starts_with('#') {
            continue;
        }

        let mut segments = trimmed.split_ascii_whitespace();
        match segments.next() {
            Some("v") => {
                let coords: Vec<f32> = segments
                    .take(2)
                    .map(|s| s.parse::<f32>().unwrap_or_else(|e| panic!("Invalid vertex coordinate in '{}': {}", line, e)))
                    .collect();
                if coords.len() >= 2 {
                    obj.vertices.push(Point2D::new(coords[0], coords[1]));
                }
            }
            Some("f") => {
                let indices: Vec<usize> = segments
                    .map(|s| {
                        let vertex_idx_str = s.split('/').next().unwrap_or(s);
                        vertex_idx_str
                            .parse::<usize>()
                            .unwrap_or_else(|e| panic!("Invalid face vertex index in '{}': {}", line, e))
                            .checked_sub(1)
                            .expect("OBJ vertex indices must be 1-based (>= 1)")
                    })
                    .collect();
                if !indices.is_empty() {
                    obj.faces.push(indices);
                }
            }
            _ => {}
        }
    }

    obj
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parse_curve_1_obj() {
        let obj = parse_obj("resources/Curve_1.obj");
        assert_eq!(obj.vertices.len(), 4);
        assert_eq!(obj.vertices[0].x, 0.0);
        assert_eq!(obj.vertices[0].y, 5.0);
        assert_eq!(obj.vertices[1].x, 5.0);
        assert_eq!(obj.vertices[1].y, 5.0);
        assert_eq!(obj.vertices[2].x, 5.0);
        assert_eq!(obj.vertices[2].y, -5.0);
        assert_eq!(obj.vertices[3].x, 0.0);
        assert_eq!(obj.vertices[3].y, -5.0);

        assert_eq!(obj.faces.len(), 1);
        assert_eq!(obj.faces[0], vec![0, 1, 2, 3]);
    }

    #[test]
    fn test_parse_bezier_curve_1_obj() {
        let obj = parse_obj("resources/BezierCurve_1.obj");
        assert_eq!(obj.vertices.len(), 4);
        assert_eq!(obj.vertices[0].x, 0.0);
        assert_eq!(obj.vertices[0].y, 5.0);
    }
}