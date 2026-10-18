use std::ops;
use bevy::prelude::*;
use crate::vector2d::Vector2D;

#[derive(Debug, Clone, Copy, PartialEq, Component)]
pub struct Point2D {
    pub x: f32,
    pub y: f32,
    e: i32,
}

impl Point2D {
    pub fn new(x: f32, y: f32) -> Self {
        Self {
            x,
            y,
            e: 1,
        }
    }

    pub fn is_point(&self) -> bool {
        self.e == 1
    }
    
    pub fn to_vector(&self) -> Vector2D {
        Vector2D::new(self.x, self.y)
    }
}

fn validate_p(p : &Point2D, v : &Point2D) {
    if !p.is_point() || !v.is_point() { panic!("Faulty data structure"); }
}


impl ops::Sub<Point2D> for Point2D {
    type Output = crate::vector2d::Vector2D;

    fn sub(self, rhs: Point2D) -> Self::Output {
        validate_p(&self, &rhs);
        crate::vector2d::Vector2D::new(self.x - rhs.x, self.y - rhs.y)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_point_subtraction() {
        let p1 = Point2D::new(5.0, 7.0);
        let p2 = Point2D::new(2.0, 3.0);
        let v = p1 - p2;
        assert_eq!(v.x, 3.0);
        assert_eq!(v.y, 4.0);
        assert!(v.is_vector());
    }
}