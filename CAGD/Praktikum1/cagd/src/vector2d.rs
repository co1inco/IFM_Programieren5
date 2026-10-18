use std::ops;
use crate::point2d::Point2D;


#[derive(Debug, Clone, Copy, PartialEq, Default)]
pub struct Vector2D {
    pub x: f32,
    pub y: f32,
    e: i32,
}

impl Vector2D {
    pub fn new(x: f32, y: f32) -> Self {
        Self {
            x,
            y,
            e: 0,
        }
    }

    pub fn is_vector(&self) -> bool {
        self.e == 0
    }

    pub fn dot(&self, rhs: &Vector2D) -> f32 {
        validate_v(self, rhs);
        self.x * rhs.x + self.y * rhs.y
    }

    pub fn to_point(&self) -> Point2D {
        Point2D::new(self.x, self.y)
    }
}

fn validate_p_v(p: &Point2D, v: &Vector2D) {
    if !p.is_point() || !v.is_vector() {
        panic!("Faulty data structure");
    }
}

fn validate_v(p: &Vector2D, v: &Vector2D) {
    if !p.is_vector() || !v.is_vector() {
        panic!("Faulty data structure");
    }
}




impl ops::Add<Vector2D> for Point2D {
    type Output = Point2D;

    fn add(self, rhs: Vector2D) -> Self::Output {
        validate_p_v(&self, &rhs);
        Point2D::new(self.x + rhs.x, self.y + rhs.y)
    }
}


impl ops::Add<Vector2D> for Vector2D {
    type Output = Vector2D;

    fn add(self, rhs: Vector2D) -> Self::Output {
        validate_v(&self, &rhs);
        Vector2D::new(self.x + rhs.x, self.y + rhs.y)
    }
}

impl ops::Sub<Vector2D> for Point2D {
    type Output = Point2D;

    fn sub(self, rhs: Vector2D) -> Self::Output {
        validate_p_v(&self, &rhs);
        Point2D::new(self.x - rhs.x, self.y - rhs.y)
    }
}



impl ops::Sub<Vector2D> for Vector2D {
    type Output = Vector2D;

    fn sub(self, rhs: Vector2D) -> Self::Output {
        validate_v(&self, &rhs);
        Vector2D::new(self.x - rhs.x, self.y - rhs.y)
    }
}

impl ops::Mul<f32> for Vector2D {
    type Output = Vector2D;

    fn mul(self, rhs: f32) -> Self::Output {
        if !self.is_vector() {
            panic!("Faulty data structure");
        }
        Vector2D::new(self.x * rhs, self.y * rhs)
    }
}

impl ops::Div<f32> for Vector2D {
    type Output = Vector2D;

    fn div(self, rhs: f32) -> Self::Output {
        if !self.is_vector() {
            panic!("Faulty data structure");
        }
        Vector2D::new(self.x / rhs, self.y / rhs)
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_vector_algebra() {
        let p = Point2D::new(1.0, 2.0);
        let v1 = Vector2D::new(3.0, 4.0);
        let v2 = Vector2D::new(1.0, -1.0);

        let p_res = p + v1;
        assert_eq!(p_res.x, 4.0);
        assert_eq!(p_res.y, 6.0);
        assert!(p_res.is_point());

        let v_add = v1 + v2;
        assert_eq!(v_add.x, 4.0);
        assert_eq!(v_add.y, 3.0);

        let v_sub = v1 - v2;
        assert_eq!(v_sub.x, 2.0);
        assert_eq!(v_sub.y, 5.0);

        let v_scale = v1 * 2.0;
        assert_eq!(v_scale.x, 6.0);
        assert_eq!(v_scale.y, 8.0);

        let dot = v1.dot(&v2);
        assert_eq!(dot, -1.0);
    }
}

