use std::ops;
use bevy::ecs::error::panic;
use crate::point2d::Point2D;


#[derive(Debug)]
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
}

fn validate_p_v(p : &Point2D, v : &Vector2D) {
    if (!p.is_point() || !v.is_vector()) { panic!("Faulty data structure"); }
}

fn validate_v(p : &Vector2D, v : &Vector2D) {
    if (!p.is_vector() || !v.is_vector()) { panic!("Faulty data structure"); }
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
        if (!self.is_vector()) { panic!("Faulty data structure"); }
        Vector2D::new(self.x * rhs, self.y * rhs)
    }
}

